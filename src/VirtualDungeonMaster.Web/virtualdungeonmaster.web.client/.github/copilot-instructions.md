# GitHub Copilot Configuration for Angular 19 Project

This file helps GitHub Copilot generate consistent, high-quality code in this Angular 19 application. It defines project structure, conventions, and prompting techniques so that suggestions align with our standards and best practices.

---

## General Guidelines

- Follow Angular 19 best practices for all code.
- Use **TypeScript** exclusively.
- Prefer `ng` CLI commands to scaffold and maintain structure.
- Follow the project's existing folder structure and naming conventions.
- Use **RxJS** for reactive and asynchronous programming.
- Ensure full compatibility with Angular 19 APIs and features.
- Write clear, concise comments above logic blocks to guide Copilot completions.

---

## File & Folder Structure

- Place **components** in: `src/app/components/`
- Place **services** in: `src/app/services/`
- Place **interfaces/models** in: `src/app/models/`
- Use **feature modules** for large, independent domains: `src/app/features/<feature-name>/`
- Group related files (component, template, style, test) together using the **Angular CLI structure**.
- Use **kebab-case** for all file and folder names.
- Avoid deep nesting—prefer flat structures within features unless clearly necessary.

---

## Coding Standards

- Use Angular’s **dependency injection** to manage services.
- Prefer **Observables** over Promises.
- Use `async` pipe in templates to handle subscriptions.
- Avoid direct DOM manipulation; use Angular **directives** and **Renderer2** when needed.
- Use **strict typing** and enable `"strict": true` in `tsconfig.json`.
- Use **Reactive Forms** where possible.
- Follow Angular’s [official style guide](https://angular.io/guide/styleguide).

---

## Best Practices

✅ DO:
- Use `ngOnInit` for component initialization.
- Write small, reusable components and delegate logic to services.
- Use `environment.ts` files for environment-specific config.
- Add clear JSDoc or inline comments to describe non-trivial logic.
- Use `HttpClient` for all HTTP calls.
- Handle errors with `catchError` in RxJS pipelines.

❌ DON’T:
- Use `any` as a type.
- Include business logic inside components.
- Use jQuery or manipulate the DOM directly.
- Create services without the `@Injectable()` decorator.
- Hardcode configuration or endpoint URLs.

---

## Prompting Tips for Copilot

To help Copilot generate accurate and context-aware code:

- Start prompts with descriptive comments:
  ```ts
  // Create a service to fetch users from backend API
  ```

- When creating features, prefix components/services with domain name:
  ```ts
  // Feature: Orders
  // Component: OrdersListComponent
  // Service: OrdersService
  ```

- Use clear typings to guide Copilot:
  ```ts
  interface Product {
    id: number;
    name: string;
    price: number;
    available: boolean;
  }

  // Filter available products
  const available = products.filter(p => p.available);
  ```

---

## Example Angular CLI Commands

- Generate a new component (inline style & with tests):
  ```bash
  ng generate component components/example --inline-style --skip-tests=false
  ```

- Generate a new service:
  ```bash
  ng generate service services/example
  ```

- Generate a feature module with routing:
  ```bash
  ng generate module features/user --route user --module app.module
  ```

- Run the dev server:
  ```bash
  npm start
  ```

- Build the application:
  ```bash
  npm run build
  ```

- Run linting and fix issues:
  ```bash
  npm run lint --fix
  ```

---

## Testing

- Write unit tests for all components, services, and pipes.
- Use **Jasmine** for writing tests and **Karma** for running them.
- Place test files next to the files they test, using `.spec.ts` suffix.
- Use `TestBed` for component and service testing.
- Maintain minimum 80% test coverage—measured with `karma-coverage`.
- Prefer shallow testing unless deep integration is necessary.
- Use mocks and spies to isolate test dependencies.

---

## Linting & Formatting

- Use **ESLint** with Angular rules (`@angular-eslint`).
- Ensure formatting with **Prettier** is consistent across the team.
- Enable auto-formatting in your IDE on save.
- Run:
  ```bash
  npm run lint
  npm run format
  ```

---

## Documentation & Comments

- Document all public APIs (services, exported functions, interfaces).
- Use JSDoc-style comments for functions and complex logic.
- Example:
  ```ts
  /**
   * Returns true if the product is available and in stock.
   */
  isAvailable(product: Product): boolean {
    return product.available && product.stock > 0;
  }
  ```

---

## Dependency Management

- Ensure all third-party packages are compatible with Angular 19.
- Avoid unnecessary dependencies—prefer built-in Angular and RxJS tools.
- Run `npm outdated` regularly to monitor dependency versions.

---

## Version Compatibility

- Project Angular version: **19.x**
- TypeScript version: ^5.x
- Node version: ≥18
- NPM version: ≥9

---

_Last updated: 2025-06-26 — For questions, refer to the [Angular Docs](https://angular.io/docs) and the [Angular Style Guide](https://angular.io/guide/styleguide)._
