const express = require('express');
const bodyParser = require('body-parser');
const MongoClient = require('mongodb').MongoClient;
const app = express();

app.use(bodyParser.urlencoded({ extended: true }));

var password_here = "10vVkcArvkRhFqGe";
var connectionString = "mongodb+srv://svetlis2000:" + password_here + "@initialpatientregistrat.kw4yj15.mongodb.net/?retryWrites=true&w=majority";

MongoClient.connect(connectionString, {
    useUnifiedTopology: true
})
    .then(client => {
        console.log('Connected to Database')
    })
    .catch(error => {
        console.error('Error connecting to database:', error)
    });


app.get('/', (req, res) => {
    res.sendFile(__dirname + '/index.html');
});

app.post('/quotes', (req, res) => {
    console.log(req.body);
});

app.listen(3000, function () {
    console.log('listening on 3000');
});
