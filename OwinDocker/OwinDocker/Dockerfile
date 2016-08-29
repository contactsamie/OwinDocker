FROM microsoft/aspnet

MAINTAINER Sam Bamgboye <contactsamie@gmail.com>  https://github.com/contactsamie/OwinDocker
LABEL description="Running Owin in docker example"

RUN apt-get update && apt-get install mono-4.0-service -y

RUN mkdir -p /app
COPY . /app
 
WORKDIR /app
CMD [ "mono",  "./bin/Release/OwinDocker.exe" ]
 
EXPOSE 9000