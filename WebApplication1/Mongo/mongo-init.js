// Аутентифицируемся как root в admin базе
db = db.getSiblingDB('admin');
db.auth('admin', 'password123');

// Создаем базу данных для приложения
db = db.getSiblingDB('blockchain_db');

// Создаем пользователя для приложения с правами на blockchain_db
db.createUser({
    user: 'app_user',
    pwd: 'app_password',
    roles: [
        {
            role: 'readWrite',
            db: 'blockchain_db'
        },
        {
            role: 'dbAdmin',
            db: 'blockchain_db'
        }
    ]
});

// Создаем коллекцию
db.createCollection('resources');

// Создаем индексы
db.resources.createIndex({ "Subject": 1 }, { unique: true });
db.resources.createIndex({ "LastUpdated": -1 });

print('MongoDB initialization completed!');