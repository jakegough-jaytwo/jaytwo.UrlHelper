FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-sdk
#FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS dotnet-runtime

FROM dotnet-sdk AS base
RUN apt-get update \
  && apt-get install -y --no-install-recommends \
    make \
    mono-devel \
  && apt-get clean \
  && apt-get autoremove\
  && rm -rf /var/lib/apt/lists/*
ENV FrameworkPathOverride /usr/lib/mono/4.5/


FROM base AS builder
WORKDIR /build
COPY . /build
RUN make clean restore build
