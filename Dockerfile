# Stage 1: Build the React client
FROM node:20-alpine AS client-build
WORKDIR /app/client
COPY client/package*.json ./
RUN npm ci
COPY client/ ./
RUN npm run build

# Stage 2: Build the .NET API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS api-build
WORKDIR /src
COPY *.slnx ./
COPY SeatReservation.Core/*.csproj SeatReservation.Core/
COPY SeatReservation.Data/*.csproj SeatReservation.Data/
COPY SeatReservation.Service/*.csproj SeatReservation.Service/
COPY SeatReservation.API/*.csproj SeatReservation.API/
RUN dotnet restore SeatReservation.API/SeatReservation.API.csproj
COPY SeatReservation.Core/ SeatReservation.Core/
COPY SeatReservation.Data/ SeatReservation.Data/
COPY SeatReservation.Service/ SeatReservation.Service/
COPY SeatReservation.API/ SeatReservation.API/
RUN dotnet publish SeatReservation.API/SeatReservation.API.csproj -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=api-build /app/publish ./
COPY --from=client-build /app/client/dist ./wwwroot/
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SeatReservation.API.dll"]
