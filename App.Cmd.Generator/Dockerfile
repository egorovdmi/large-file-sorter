FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY . ./

WORKDIR /app/App.Cmd.Generator
RUN dotnet restore

VOLUME /Data

CMD ["dotnet", "run"]
