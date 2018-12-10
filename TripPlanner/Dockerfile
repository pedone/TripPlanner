#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 50099
EXPOSE 44365

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["TripPlanner/TripPlanner.csproj", "TripPlanner/"]
RUN dotnet restore "TripPlanner/TripPlanner.csproj"
COPY . .
WORKDIR "/src/TripPlanner"
RUN dotnet build "TripPlanner.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TripPlanner.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TripPlanner.dll"]