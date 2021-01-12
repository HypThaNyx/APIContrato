<h2 align="center">
  APIContrato
</h2>

<p align="center">
  This is a public project created to test basic knowledge on ASP.NET Core 3.0, WebAPIs and cache management.
</p>

<p align="center">
  <a href="https://github.com/HypThaNyx">
    <img alt="Made by Wesley Yago" src="https://img.shields.io/badge/made%20by-Wesley%20Yago-orange">
  </a>

  <img alt="Last Commit" src="https://img.shields.io/github/last-commit/HypThaNyx/APIContrato">

  <img alt="Contributors" src="https://img.shields.io/github/contributors/HypThaNyx/APIContrato">

  <img alt="License" src="https://img.shields.io/badge/license-MIT-orange">
</p>

---

## Table of Contents

<ul>
  <li><a href="#-getting-started">Getting Started</a></li>
  <li><a href="#-features">Features</a></li>
  <li><a href="#-support">Support</a></li>
  <li><a href="#-license">License</a></li>
</ul>

---

## 🚀 Getting Started

### Prerequisites

- [.NET Core 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

### Setup

Instructions to use:
- download or clone the repo
- run a Shell prompt inside the project's service folder (APIContrato/APIService)
- enter the following command and press Enter:

```
dotnet run
```

- open any browser and go to the website hosted at [https://localhost:5001/](https://localhost:5001/)
- use Swagger to communicate with the API and make requests

---

## 📋 Features

The API should contain:
- [X] entity Contrato, with: id (auto-increment), data contratação, quantidade de parcelas, valor financiado, prestações.
- [X] entity Prestação, with: contrato, data vencimento, data pagamento, valor, status (Aberta, Baixada, Atrasada).
- [X] the Status field should be displayed based on the field *data vencimento*, *data atual* and *data pagamento*, not being stored on the database.
    - *data vencimento* >= *data atual* && !*data pagamento* = Aberta
    - *data vencimento* < *data atual* && !*data pagamento* = Atrasada
    - *data pagamento* = Baixada 
- [X] *InMemoryDB* to store data in memory.

Also, the API should:
- [X] use MVC through modeling.
- [X] be a RESTful API (with basic CRUD).
- [ ] apply Clean Code, SOLID and programming practices.
- [ ] perform Unit Testing.
- [X] not allow the user to send the ID along with the contract's requisition.
- [ ] use InMemoryCache, making the cache expire after midnight on the next day.
- [X] use Swagger.
- [ ] use Feature Flags.

The main idiom used is Brazilian Portuguese, my mother language.

---

### Documentation

This project uses the following Commit Guidelines:

- build: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
- ci: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
- docs: Documentation only changes
- feat: A new feature
- fix: A bug fix
- perf: A code change that improves performance
- refactor: A code change that neither fixes a bug nor adds a feature
- style: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
- test: Adding missing tests or correcting existing tests

---

## 📌 Support

Contact and support me through the following social medias:

- <a href="https://hypthanyx.itch.io/">
    <img alt="Check out my Itch.io!" src="https://img.shields.io/badge/Itch.io-HypThaNyx-fff?logo=itch.io&style=social">
  </a>
- <a href="https://twitter.com/hypthanyx">
    <img alt="Check out my Twitter!" src="https://img.shields.io/badge/Twitter-HypThaNyx-fff?logo=twitter&style=social">
  </a>
- <a href="https://www.instagram.com/hypthanyx/">
    <img alt="Check out my Instagram!" src="https://img.shields.io/badge/Instagram-HypThaNyx-fff?logo=instagram&style=social">
  </a>
- <a href="https://www.linkedin.com/in/wesley-yago-da-silva/">
    <img alt="Check out my LinkedIn!" src="https://img.shields.io/badge/LinkedIn-Wesley Yago-black.svg?logo=linkedin&color=666&style=social">
  </a>
- <a href="https://www.youtube.com/channel/UC_x5u0TqJWN4O3GMwZRWkrg">
    <img alt="Check out my YouTube!" src="https://img.shields.io/badge/YouTube-HypThaNyx-black.svg?logo=youtube&color=666&style=social">
  </a>

---

## 📝 License

<img alt="License" src="https://img.shields.io/badge/license-MIT-%2304D361">

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

🧰 Being developed by Wesley Yago!