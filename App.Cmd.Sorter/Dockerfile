FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY . ./

WORKDIR /app/App.Cmd.Sorter
RUN dotnet restore

VOLUME /app/Data

CMD ["dotnet", "run"]
