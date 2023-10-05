# ElsaSandbox

An Elsa testbed

## ElsaBookmarkTest

The site is an Elsa server, and the process used within the site is to call the underlying Elsa services directly.
This is ok for a single website, but you can't do that in a distributed environment, otherwise you will have multiple Elsa instances all running against the same DB.
If you do do this, then you need to implement some kind of sync/messaging bus between the Elsa instances to allow them to communicate with one another.

The workflows are loaded from the Workflows directory (using the BlobStorage provider supplied with Storage.Net), and Swagger UI is available to see the resulting API methods on the Elsa server.
The Elsa Designer dashbaord is available in the menu also.

## ElsaBookmarkViaClient

This site is a plain website with the inclusion of the ElsaExtended ElsaClient, the features and functionailty are the same as ElsaBookmarkTest but we are using an ElsaClient derivative to implement those features/functions.

It is dependant on the ElsaBookmarkTest website running the Elsa Server API.

## Elsa.Client.Extended

This is an extended version of the built-in ElsaClient.  Since the built-in version is lagging behind on quite a few things, and there are many missing features, this implementation adds some new endpoints to and Elsa server.
I've tried to maintain the same structure as the original source and where I've overridden the Elsa.Client implementations, they are under the Services folder

- `IUserTasksApi` - Sipke added the UserTaskSercice in the recent history, and its extremely easy to use and effective, however it does have a dependency on the Elsa.Activities.UserTask package (since that's where the `UserTask` activity is defined naturally). The code for this has been placed inside it's own project, `Elsa.Activities.UserTask.Api` so that dependency can be used or not.
- `IWorkflowInstancesApi` & `IWorkflowRegistryApi` - since we cannot extend an interface, they are copies of the code from the Elsa library but with additional methods added to implement
	- missing methods	
	- new methods implemented in `Elsa.Server.Api.Extended`

### Missing Methods

The missing methods are Elsa.Client implementations of API methods that already exist in the Elsa.Server.Api codebase, but for whatever reason do not have Elsa.Client implementaions 

### New Methods

The new methods are Elsa.Client implementations of API methods that do NOT already exist in the Elsa.Server.Api codebase but I've implemented in the `Elsa.Server.Api.Extended` project

## Elsa.Activities.UserTask.Api

This library provides the Endpoints for the UserTask methods.  it is dependant on the `Elsa.Activities.UserTask` package for the `UserTask` activity, and the `Elsa.Server.Api` for the underlying code structure for cerating Elsa Api endpoints.
It has been separated out to it's own class library due to the dependency on the `Elsa.Activities.UserTask` package.

## Elsa.Server.Api.Extended

This library provides additional methods for the existing Elsa.Client interfaces that don't exist in the `Elsa.Server.Api`, such as:
- Get Workflow by Name
- Get Workflow by Tag
- Get Workflow Instance by CorrelationId
- Get Workflow Instances (Full Details)

