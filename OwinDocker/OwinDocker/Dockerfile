#https://www.docker.com/products/docker-toolbox
#https://docs.docker.com/docker-for-windows/
#https://docs.docker.com/engine/reference/builder/
# http://localhost:9000/api/hello
 
# We use the microsoft/aspnet image as a starting point.
FROM microsoft/aspnet
 
#docker ps -a
#docker images
#
#docker rm $(docker ps -a -q)
#docker rmi $(docker images -q)
#
 
#docker build -t owindocker:v1 .
#docker run -t -i -d -P owindocker:v1
 
RUN apt-get update && apt-get install mono-4.0-service -y
 
RUN mkdir -p /app
COPY . /app
 
WORKDIR /app
CMD [ "mono",  "./OwinDocker.exe" ]
 
EXPOSE 9000:9000