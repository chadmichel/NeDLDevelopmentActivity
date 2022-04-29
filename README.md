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
   ng g s Backend

3. Modify the backend service backend.service.ts file to return some static fake data.

4. Modify the component TypeScript to call the backend service to get the dat.

5. Modify the template.

```
<h1>Shopping List</h1>
<ol>
    <li *ngFor="let item of shoppingList">{{item.title}}</li>
</ol>
```
