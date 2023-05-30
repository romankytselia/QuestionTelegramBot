FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuestionTelegramBot/QuestionTelegramBot.csproj", "QuestionTelegramBot/"]
RUN dotnet restore "QuestionTelegramBot/QuestionTelegramBot.csproj"
COPY . .
WORKDIR "/src/QuestionTelegramBot"
RUN dotnet build "QuestionTelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuestionTelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestionTelegramBot.dll"]
