# school-scheduler

Simple app to make school schedule powered by: 
* ASP.NET Core Web Api and Entity Framework with handling optimistic concurrency server side 
* React on client side

<br/>

## Server

### requirements
.NET Core 5.0<br/>
Entity Framework 5.0 <br/>
SQL Server<br/>


### launch guide
```bash
$ cd ./school-scheduler/SchoolScheduler/WebApi
$ dotnet run
```

### unit test launch guide
```bash
$ cd ./school-scheduler/SchoolScheduler/UnitTests
$ dotnet test
```

## Client

## Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

### Available Scripts

In the project directory, you can run:

#### `yarn start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

#### `yarn test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

#### `yarn build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

