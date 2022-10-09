# Auto
Демонстрация 5-ти различных взаимодействий: API, GraphQL, RabbitMQ, 
Установка docker для RabbitMQ:
1.Локально для теста развернуть в docker [ Install Docker Engine | Docker Documentation](https://docs.docker.com/engine/install/)
2. Скачать в store Ubuntu 22.04.1 LTS
3. Проверить версию "wsl -l -v" (если 2, всё ок, иначе docker сам предложит установить 2 версию)
4. Ввести в командную строку Windows: docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 8080:15672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=rabbitmq rabbitmq:3-management
