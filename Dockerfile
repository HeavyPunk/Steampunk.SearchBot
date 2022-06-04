FROM bitnami/dotnet:6.0-debian-10
COPY ./bin/ /usr/src/
WORKDIR /usr/src/
CMD ./SearchBot.Worker
