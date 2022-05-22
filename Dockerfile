FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS builder

WORKDIR /src
COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as final

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y newrelic-netcore20-agent \
&& rm -rf /var/lib/apt/lists/*

# Enable agent
ENV CORECLR_ENABLE_PROFILING=1 
ENV CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A}
ENV CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent
ENV CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so
ENV NEW_RELIC_LICENSE_KEY=71bdf3f1c0701dd0bb86290da75b368d58b0NRAL
ENV NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true
ENV NEW_RELIC_APP_NAME="ObservabilityDemo.UsersApi"

WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "ObservabilityDemo.UsersApi.dll"]