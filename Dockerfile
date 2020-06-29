FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-sdk
#FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS dotnet-runtime

FROM dotnet-sdk AS base

RUN apt-get update \
  && apt-get install -y --no-install-recommends \
# Install Make for makefile support
    make \
# Install Mono (for .NET Framework build target)
    mono-devel \
  && apt-get clean \
  && apt-get autoremove \
  && rm -rf /var/lib/apt/lists/*

ENV FrameworkPathOverride /usr/lib/mono/4.5/
ENV PATH="${PATH}:/root/.dotnet/tools"

FROM base AS builder
WORKDIR /build
COPY . /build
RUN make deps restore build
