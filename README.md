# NeDLDevelopmentActivity

Tool chain walk through created for NeDL program.

## Related Blog Posts

- https://dontpaniclabs.com/blog/post/2022/05/17/top-to-bottom-with-angular-and-net-core/
- https://dontpaniclabs.com/blog/post/2022/06/16/top-to-more-bottom-with-angular-and-net-core/

## Activity 1

Create a very simple HTML web page for a shopping list.

1. Install the http-server npm module. This module makes hosting a static site very easy across many platforms.
   npm install -g http-server

2. Now we need to create a simple HTML page to display a shopping list.

```
        <h1>Shopping List</h1>
        <ol>
            <li>Milk</li>
            <li>Eggs</li>
            <li>Stick of butter</li>
        </ol>
```

3. Run the node http server. This will run a lightweight server that is easy to start and stop.
   http-server

![Shopping List Example](https://raw.githubusercontent.com/chadmichel/NeDLDevelopmentActivity/main/Images/ShoppingListExample.png)

## Activity 2

Recreate the shopping list as an Angular application.

1. Install Angular
   npm install -g @angular/cli

2. Create new Angular project
   ng n ShoppingListAngular

3. Change directory into the directory for the Angular application.
   cd ShoppingListAngular

4. Run the Angular application
   ng serve

5. Open your web browser to http://localhost:4200/

6. Let's add a new component to our Angular Application
   ng g c MyList

7. Open the source code for the Angular application in VS Code.

8. Simplify the app.component.html

```
<router-outlet></router-outlet>
```

8. Create a new default route for the MyList component. This is done by modifying the app-routing.module.ts.

```
const routes: Routes = [{ path: '', component: MyListComponent }];
```

9. Modify the my-list.component.html file to display a shopping list.

```
        <h1>Shopping List</h1>
        <ol>
            <li>Milk</li>
            <li>Eggs</li>
            <li>Stick of butter</li>
        </ol>
```

10. Refresh the web page (http://localhost:4200)

## Activity 3

Use data binding to display the shopping list.

1. Add a new file to your Shopping List Angular application. This file will be the Data Transfer Object used to pass data to our UI component.

```
export interface ShoppingListItem {
    title: string;
}
```

2. Generate a service for backend communication.

```
ng g s Backend
```

3. Modify the backend service backend.service.ts file to return some static fake data.

```
export class BackendService {
  constructor() {}

  async shoppingList(): Promise<ShoppingListItem[]> {
    return new Promise((resolve) => {
      resolve([
        { title: 'Milk' },
        { title: 'Eggs' },
        { title: 'Stick of Butter' },
      ] as ShoppingListItem[]);
    });
  }
}
```

4. Modify the component TypeScript to call the backend service to get the dat.

```
export class MyListComponent implements OnInit {
  shoppingList: ShoppingListItem[] | undefined;

  constructor(private backend: BackendService) {}

  async ngOnInit() {
    this.shoppingList = await this.backend.shoppingList();
    console.log(JSON.stringify(this.shoppingList));
  }
}
```

5. Modify the template to use databinding to display the data.

```
<h1>Shopping List</h1>
<ol>
    <li *ngFor="let item of shoppingList">{{item.title}}</li>
</ol>
```

## Activity 4

Create a .NET Core Backend to return the shopping list.

![Backend Service Image](https://github.com/chadmichel/NeDLDevelopmentActivity/blob/main/Images/BackendSource.png)

1. Create a new .NET Core project.

```
dotnet new webapi -n Backend
```

2. Add a DTO for the shopping list items.

```
public class ShoppingListItem
{
    public string Title { get; set; }
}
```

3. Create a new ShoppingListController in our .NET project. Controllers should go in the Controller folder.

```
[ApiController]
[Route("[controller]")]
public class ShoppingListController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public ShoppingListController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetShoppingList")]
    public ShoppingListItem[] Get()
    {
        var items = new ShoppingListItem[] {
            new ShoppingListItem()
            {
                Title = "Milk"
            },
            new ShoppingListItem()
            {
                Title = "Eggs"
            },
            new ShoppingListItem()
            {
                Title = "Stick of Butter"
            }
        };
        return items;
    }
}
```

5. Because our Angular application will be running on a different hostname, we will need to enable CORS. We should only enable CORS for the hostname of our Angular application, but for this simple demo we will enable CORS for all domains.
   Why do we have to do this? What is CORS? CORS is a security mechanism that browsers use to help prevent malicious scripts from running code without a user’s knowledge. With CORS, we restrict our applications as much as possible.

```
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x =>
x.AllowAnyMethod()
.AllowAnyOrigin()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.MapControllers();

app.Run();
```

6. After we have Shopping List controller we will test the controller using Postman. To do this this lets run the dotnet application in VS Code.

7. Run the postman application. Create a request that runs against "https://localhost:7027/ShoppingList".

![Postman example](https://raw.githubusercontent.com/chadmichel/NeDLDevelopmentActivity/main/Images/Postman.png)

## Activity 5

Wire up the Angular application to the .NET backend.

1. Modify the Angular app.module.ts file to include the HttpClientModule.

```
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MyListComponent } from './my-list/my-list.component';

import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [AppComponent, MyListComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
```

2. Modify the Angular backend service backend.service.ts file to call our .NET Core backend.

```
export class BackendService {
  constructor(private http: HttpClient) {}

  async shoppingList(): Promise<ShoppingListItem[]> {
    return firstValueFrom(
      this.http.get<ShoppingListItem[]>('https://localhost:7027/ShoppingList')
    );
  }
}
```

Now we have an Angular application calling a .NET Core application. Lot of steps getting this going, but if you can get this running this same pattern can build a lot of software.

## Activity 6

The previous series of activities got us an Angular application running making an HTTP call to a .NET Core backend to retrieve a shopping list.

In this activity we will extend our .NET backend to use SQL Server.

First, lets create a Shopping database.

```
create Database Shopping
```

Next we need to create a ShoppingListItems table.

```
create table ShoppingListItems
(
    Id int IDENTITY(1,1) primary key clustered,
    Title nvarchar(100)
)
```

Now that we have a database table. We need to insert some data. We can achieve this just straight SQL.

```
insert into ShoppingListItems
    (Title)
values
    ('Milk'),
    ('Eggs'),
    ('Stick of Butter')
```

Let's select our data to make sure we inserted it correctly.

```
select * from ShoppingListItems;
```

Now we need to update our .NET backend to query our database. There are a lot of options to achieve this goal. In this case we will use Entity Framework to retrieve the data.

For our .NET application to use Entity Framework we will first have to add a reference to Entity Framework to our project. To do this we can use the .NET CLI to include the nuget package for EntityFramework.

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Now we need to create a class for our EntityFramework database context.

```
using Microsoft.EntityFrameworkCore;

public class ShoppingDatabase : DbContext
{

    public ShoppingDatabase(DbContextOptions<ShoppingDatabase> options) : base(options)
    {

    }

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }
}
```

Now that we have to Database context we must wire it up into the .NET Core's dependency services.

```
// Configure the database context service.
builder.Services.AddDbContext<ShoppingDatabase>(options =>
    options.UseSqlServer("<YOUR CONNECTION STRING>"));
```

The controller ShoppingListController can be updated to use the database context to retrieve the items from the database.

```
public class ShoppingListController : ControllerBase
{
    ShoppingDatabase Database { get; set; }

    public ShoppingListController(ShoppingDatabase database)
    {
        Database = database;
    }

    [HttpGet(Name = "GetShoppingList")]
    public ShoppingListItem[] Get()
    {
        return Database.ShoppingListItems.ToArray();
    }
}
```

Now we should have an Angular front end. Calling a .NET Core backend. Communnicating with a SQL Server data store. This pattern / setup is a good starting point for understanding much of modern application hosting.
