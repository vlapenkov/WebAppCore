1. Создаем проект с поддепржкой identity
2. Меняем connection string
3. dotnet ef migrations add [migration name] 
  dotnet ef database update - создает БД и применяет миграции
4. Для bundling и minification  достаточно добавить plugin bundle & minify
5. PagedList есть в PeopleController/List


6. После всего переделал проект на использование Webpack (вместо bower)
https://habr.com/post/328638/

7. После пункта 6 переделал на использвание TypeScript, но пока что не подружил с jquery

8. Добавил OWIN с связь с Google

9. Добавил signalR в этом же решении в новом проекте

10. Меняем культуру на лету через Middleware

11. GlobalException filter

12. Добавил PagedList в People/PagedList
