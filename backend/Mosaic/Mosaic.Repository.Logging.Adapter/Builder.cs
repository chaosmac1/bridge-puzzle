using System.Diagnostics;
using System.Reflection;

namespace Mosaic.Repository.Logging.Adapter;

public static class Builder {
    
    public static NLog.Logger GetCurrentClassLogger() {
        int framesToSkip = 2;

        string className = string.Empty;
        var stackFrame = new StackFrame(framesToSkip, false);
        className = GetClassFullName(stackFrame);
        
        return NLog.LogManager.GetLogger(className);
    }
    
    private static string GetClassFullName(StackFrame stackFrame)
    {
        string className = LookupClassNameFromStackFrame(stackFrame);
        if (string.IsNullOrEmpty(className))
        {
            var stackTrace = new StackTrace(false);
            className = GetClassFullName(stackTrace);
            if (string.IsNullOrEmpty(className))
            {
                var method = GetStackMethod(stackFrame);
                className = method?.Name ?? string.Empty;
            }                    
        }
        return className;
    }
    
    private static string GetClassFullName(StackTrace stackTrace)
    {
        foreach (StackFrame frame in stackTrace.GetFrames())
        {
            string className = LookupClassNameFromStackFrame(frame);
            if (!string.IsNullOrEmpty(className))
            {
                return className;
            }
        }
        return string.Empty;
    }
    
    private static string LookupClassNameFromStackFrame(StackFrame stackFrame)
    {
        var method = GetStackMethod(stackFrame);
        if (method != null && LookupAssemblyFromMethod(method) != null)
        {
            string className = GetStackFrameMethodClassName(method, true, true, true);
            if (!string.IsNullOrEmpty(className))
            {
                if (!className.StartsWith("System.", StringComparison.Ordinal))
                    return className;
            }
            else
            {
                className = method.Name ?? string.Empty;
                if (className != "lambda_method" && className != "MoveNext")
                    return className;
            }
        }

        return string.Empty;
    }
    
    public static MethodBase GetStackMethod(StackFrame stackFrame)
    {
        return stackFrame?.GetMethod();
    }
    
    private static Assembly LookupAssemblyFromMethod(System.Reflection.MethodBase method)
    {
        var assembly = method?.DeclaringType.Assembly ?? method?.Module?.Assembly;
        
        return assembly;
    }
    
    private static string GetStackFrameMethodClassName(MethodBase method, bool includeNameSpace, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
    {
        if (method is null)
            return null;

        var callerClassType = method.DeclaringType;
        if (cleanAsyncMoveNext
            && method.Name == "MoveNext"
            && callerClassType?.DeclaringType != null
            && callerClassType.Name?.IndexOf('<') == 0
            && callerClassType.Name.IndexOf('>', 1) > 1)
        {
            // NLog.UnitTests.LayoutRenderers.CallSiteTests+<CleanNamesOfAsyncContinuations>d_3'1
            callerClassType = callerClassType.DeclaringType;
        }

        string className = includeNameSpace ? callerClassType?.FullName : callerClassType?.Name;
        if (cleanAnonymousDelegates && className?.IndexOf("<>", StringComparison.Ordinal) >= 0)
        {
            if (!includeNameSpace && callerClassType.DeclaringType != null && callerClassType.IsNested)
            {
                className = callerClassType.DeclaringType.Name;
            }
            else
            {
                // NLog.UnitTests.LayoutRenderers.CallSiteTests+<>c__DisplayClassa
                int index = className.IndexOf("+<>", StringComparison.Ordinal);
                if (index >= 0)
                {
                    className = className.Substring(0, index);
                }
            }
        }

        if (includeNameSpace && className?.IndexOf('.') == -1)
        {
            var typeNamespace = GetNamespaceFromTypeAssembly(callerClassType);
            className = string.IsNullOrEmpty(typeNamespace) ? className : string.Concat(typeNamespace, ".", className);
        }

        return className;
    }
    
    private static string? GetNamespaceFromTypeAssembly(Type callerClassType)
    {
        var classAssembly = callerClassType.Assembly;
        if (classAssembly is not null)
        {
            var assemblyFullName = classAssembly.FullName;
            if (assemblyFullName?.IndexOf(',') >= 0 && !assemblyFullName.StartsWith("System.", StringComparison.Ordinal) && !assemblyFullName.StartsWith("Microsoft.", StringComparison.Ordinal))
            {
                return assemblyFullName.Substring(0, assemblyFullName.IndexOf(','));
            }
        }

        return null;
    }
}