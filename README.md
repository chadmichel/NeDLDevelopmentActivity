# NeDLDevelopmentActivity

Tool chain walk through created for NeDL program.

## Activity 1

Create a very simple HTML web page for a shopping list.

1. Install http-server
   npm install -g http-server

2. Create a simple HTML page to display a shopping list

```
        <h1>Shopping List</h1>
        <ol>
            <li>Milk</li>
            <li>Eggs</li>
            <li>Stick of butter</li>
        </ol>
```

3. Run http-server
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

5. Modify the template.

```
<h1>Shopping List</h1>
<ol>
    <li *ngFor="let item of shoppingList">{{item.title}}</li>
</ol>
```

## Activity 4

Create a .NET Core Backend to return the shopping list.

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

3. Create a new ShoppingListController.

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

5. Because our Angular application will be running on a different hostname we will need to enable CORS. We should only enable CORS for the hostname of our Angular application, but for this simple demo we will enable CORS for all domains. To accomplish this change the program.cs file to enable CORS.

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
