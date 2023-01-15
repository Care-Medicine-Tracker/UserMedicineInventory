# UserMedicineInventory
 ## Introduction
 The User Medicine Inventory Service is in charge of all registered medicines belonging to users to the Care web-application. This service manages adding, updating, and removing users information regarding their medicines they take from the system. For more information regarding the project and the architecture can it be found at **[Medicine-Tracker](https://github.com/vcaf/Medicine-Tracker)** repository. 

This readme file will cover the endpoints of the service and running the service with its database on docker.

## Endpoints
![image](https://user-images.githubusercontent.com/78371221/212545101-2d182194-d102-486d-9e07-cb894a7696fc.png)
<b><i>Image 1: Endpoints of the User Medicine Inventory Service in Swagger</i></b>

| Endpoints | Description | 
|--|--|
|(GET) /prescription/user/{userId}|retrieves all medicines registered to a specific user.|
|(POST) /prescription| creates a new prescription for a user.|
| (PUT) /prescription/{id} | updates a prescription for a user. |
|(DELETE) /prescription/{id}| deletes a prescription for a user. |


## Build steps for running service on Docker
The build steps for running the service with the mongo database on Docker. 

<img width="940" alt="mongo-command" src="https://user-images.githubusercontent.com/78371221/211282427-767ffabf-65bf-4e71-bfbb-f0fb5734d207.PNG">
<b><i>Image 2: Command to create mongo container</i></b>
 
1. Pull the mongo image from docker with the assigned port and volume to store data with using the following command "Docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo"

  - p: to reach the mongodb container we set the external and internal port to 27017.
  - v: the volumes specify how we will be storing the mongodb database files.
 
![docker-ps](https://user-images.githubusercontent.com/78371221/211282275-7c3c9c75-3d08-4c8b-b0d7-964865bc2bec.png)
<b><i>Image 3: command to confirm image is running</i></b>
 
2. Confirm that the docker image is being used "docker ps"


3. Pull the project from GitHub on your device in the IDE you are working with. (I will be using Visual Studio Code)

<img width="960" alt="Screenshot (1407)" src="https://user-images.githubusercontent.com/78371221/209117837-4cff223b-bf0f-41cc-b64c-ce6a9427d196.png">
<b><i>Image 4: VSCode build of service</i></b>
 
4.  Build the project in the IDE of choice. 


![locally (2)](https://user-images.githubusercontent.com/78371221/211284833-337b75ba-39f6-49d4-8f73-6e2aa9e5adee.png)
<b><i>Image 5: User Medicine Inventory service running locally</i></b>
 
5.  The project is now running locally and should be able to be found at https://localhost:5005. 

![Screenshot (1414)](https://user-images.githubusercontent.com/78371221/209119018-271e6929-beca-44ba-b4fc-5994f98d4f6b.png)
<b><i>Image 6: Swagger page of the User Medicine Inventory service</i></b>
 
6.  To be able to see the endpoints we can use Swagger by adding "/swagger/index.html" to the url. Example "https://localhost:5005/swagger/index.html" as shown in image
