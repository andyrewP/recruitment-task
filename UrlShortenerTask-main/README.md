## Url Shortener WebApplication
This is a web application to shorten URLs. It provides the functionality for creating and displaying the history of created short URLs.

## Technologies Used

* ASP.NET Core
* FluentValidation
* X.PagedList
* Entity Framework in-memory DB
* XUnit


## Getting Started
To get started with this application, follow these steps:

1. Clone this repository to your local machine.
2. Make sure you have .NET Core installed on your machine.
3. Open the project in your favorite code editor.
4. Run the following command in the terminal or command prompt at the root of the project to restore the necessary packages:
```dotnet restore```
5. After the packages have been restored, navigate to `recruitment-task\UrlShortenerTask-main\src\UrlShortener.WebApplication` catalog and 
run the following command to start the application:
```dotnet run```
6. Open a web browser and go to the port specified in your console window. You should see the home page of the application.
That's it! You can now use the application.

## Usage
1. Navigate to `Url Shortener` page in the nav bar.
2. The page will display a list of previously added short urls (blank during inital startup)
3. Click on the `Create New Shorted URL` button.
4. Paste or type a valid URL you want to shorten.
5. You may optionally provide your own unique alias for the url.
6. The shortened URL will be displayed in a table. You can copy the URL to your clipboard by clicking the "Copy" button.

You click on one of the shorted URLs to verify whether proper redirection works.

## Notes
This application uses client-side validation and FluentValidation to validate incoming requests. If a request fails validation, the user will be redirected to the Create page with an error message.

Pagination is implemented using X.PagedList. By default, 3 items will be shown per page.

If an error occurs, the user will be redirected to the Home page with an error message.
