FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
WORKDIR /src 
COPY LorryTransport.Domain/*.csproj LorryTransport.Domain/ 
COPY LorryTransport.Application/*.csproj LorryTransport.Application/ 
COPY LorryTransport.Infrastructure/*.csproj LorryTransport.Infrastructure/ 
COPY LorryTransport.API/*.csproj LorryTransport.API/ 
RUN dotnet restore LorryTransport.API/LorryTransport.API.csproj 
COPY . . 
RUN dotnet publish LorryTransport.API/LorryTransport.API.csproj -c Release -o /app/out 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime 
WORKDIR /app 
COPY --from=build /app/out . 
ENTRYPOINT ["dotnet", "LorryTransport.API.dll"] 
