### Create mongo container

```docker run --name mongo --interactive --publish 27017:27017  -d mongodb/mongodb-community-server:latest```

### Start mongo container
```docker start --name mongo```

### Connect to mongo container
```docker exec -it mongo mongosh```

### Stop mongo container
```docker stop --name mongo```



