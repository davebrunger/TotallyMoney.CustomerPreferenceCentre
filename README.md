# TotallyMoney.CustomerPreferenceCentre
Solution for the Totally Money Customer Preference Centre Technical Exercise
## Prerequisites
To run the functionality locally install the *Azure Functions Core Tools*
## Usage
When running the functions locally, the developer console will display the URL of the Swagger Api frontend. The function can easily be called through this. Alternatively the URL of the function itself will also be displayed in the console. You can make a post request directly to that URL to call the API. In either case the list of customer preferences should be in a format similar to the following in the POST's body:

    [
        {
            "name": "David",
            "preference": {
                "type" : "specificDate",
                "specificDate" : 12
            }
        },
        {
            "name": "Simon",
            "preference": {
                "type" : "daysOfWeek",
                "daysOfWeek" : ["Monday", "Tuesday", "Saturday"]
            }
        },
        {
            "name": "Mike",
            "preference": {
                "type" : "never"
            }
        },
        {
            "name": "Carina",
            "preference": {
                "type" : "everyDay"
            }
        }
    ]


## Technologies
The API is written as an Azure Function to demonstate how a function like this could be deployed as a serverless application. The OpenAPI attributes allow the Azure Function to provide a handy bare bones UI for calling the function. The OneOf library is used to provide functionality to C# similar to the Discriminated Union feature in F# which is ideal for representing this kind of data.
## Missing Functionality
Obviously at the moment there is noe nice looking UI. My next step would be to create a simple frontend in ReactJS and Typescript. I'd like to add a decent example to the Swagger UI, but I have not been able to work out how to provide an example using the OneOf library in a reasonable amount of time.

The next step after creating the UI would be to add a CosmosDb so that customers could be added and reports run as needed.