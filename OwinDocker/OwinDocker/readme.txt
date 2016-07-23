
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
 
EXPOSE 9000




Use the docker login command to log into the Docker Hub from the command line.
The format for the login command is:
docker login --username=yourhubusername --email=youremail@company.com
When prompted, enter your password and press enter. So, for example:
$ docker login --username=maryatdocker --email=mary@docker.com
Password:
WARNING: login credentials saved in C:\Users\sven\.docker\config.json
Login Succeeded


$ docker tag 7d9495d03763 maryatdocker/docker-whale:latest


#docker tag owindocker contactsamie/owindocker
#docker push  contactsamie/owindocker
