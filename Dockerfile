FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnet-sdk

FROM dotnet-sdk AS base

RUN apt-get update \
  && apt-get install -y --no-install-recommends \
# Install Make for makefile support
    make \
  && apt-get clean \
  && apt-get autoremove \
  && rm -rf /var/lib/apt/lists/*

ENV PATH="${PATH}:/root/.dotnet/tools"

FROM base AS builder
WORKDIR /build
COPY . /build
RUN make deps restore build
