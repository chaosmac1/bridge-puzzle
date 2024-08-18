using FastEndpoints;
using Mosaic.Init;

InitializeRepository.Create().Run();

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints(Options);

void Options(EndpointDiscoveryOptions config) {
    config.Filter = type => {
        return true;
    };
    
    InitializeSlices.Create(config).Bind();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFastEndpoints();

app.Run();