FROM microsoft/aspnet

RUN apt-get update && apt-get install mono-4.0-service -y

RUN mkdir -p /app
COPY . /app
 
WORKDIR /app
CMD [ "mono",  "./OwinDocker.exe" ]
 
EXPOSE 9000