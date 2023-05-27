# Health & Fitness Journal Portal

1.  Prerequisites Installations:
    1.	Visual Studio Community Version 2022 (Free Edition) Download 
    2.	.Net Core framework (Latest 7) Download
    3.	.Net hosting setup: dotnet-hosting-7.0.5-win.exe Download
    4.	Microsoft SQL Server  2022 Express  Download
    5.	IIS Server should be configured

2.	Build HealthFit Project Solution
    1.	Download the Repository from the git. Github Download
    2.	Navigate to \Source Code\HealthFit APIs
    3.	Open the Solution HealthFit APIs.sln (Make sure Visual Studio should be run as administrator)
    4.	Build the solution unless the Build Successful message not show up. (Nuget packages will get restored automatically)
    5.	Navigate to \Source Code\HealthFit Web folder 
    6.	Open the Solution HealthFit Web.sln (Make sure Visual Studio should be run as administrator)
    7.	Build the solution unless the Build successfully message not show up. (Nuget packages will get restored automatically)

3.	Configure the File Server Shared Path and Database Connection string :
    1.	This is shared path to save and retrieve file and being used in HealthFIt web portal.
    2.	Navigate to any drive on your local computer. 
    3.	Create a Folder name as “HealthFit File Server”
    4.	Then Right Click on Folder and share the folder with Everyone as access right. (if this is created property you will not able access web portal)
    5.	Copy the Shared folder path and add for key “FileServerPath” into “HealthFit APIs” appsettings.json file 
    6.	Update the database connection string value “HealthFitDBConnectionString” to your database server hostname ,as we are using Entity Framework , database schema will be automatically generated after project starts

4.	Configure the File Server Path as website in Application
    1.	Open the IIS Server 
    2.	Go To the Site - > Default Web Site - > Add Application and copy the shared folder path (Step 3.5) to Physical path of this web site.
    3.	Browse the website , this may give you error but ignore for now as now contents is there.
    4.	Copy the File Server url path e.g. http://localhost/HealthFitFileServer/ 
    5.	Copy the Shared folder path and add for key “FileServerBaseUrl” into “HealthFit Web” appsettings.json file.
