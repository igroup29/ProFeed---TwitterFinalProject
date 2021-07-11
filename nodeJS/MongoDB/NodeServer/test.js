const express = require('express');
const app = express();
const port = 8585;



var MongoClient = require('mongodb').MongoClient;
var urlGET = "mongodb+srv://systemAdmin:admin149@global.0jqdg.mongodb.net/SearchKeys?retryWrites=true&w=majority";
var urlPOST = "mongodb+srv://systemAdmin:admin149@global.0jqdg.mongodb.net/Searchs?retryWrites=true&w=majority";

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// GET method route
app.get('/SearchKeys', (req, res) => {
    MongoClient.connect(urlGET, function(err, db) {
        if (err) throw err;
        var dbo = db.db("SearchKeys");
        
        dbo.collection(req.query.collection).find({}).toArray(function(err, result) {
            if (err) throw err; 
            console.log(result);
            db.close();
            res.send(JSON.stringify(result));
        });
        
    }); 
})

// POST method route
// http://localhost:8585/Search?type=<builder/advenced>&mainCategory=<the main category with hashtag>&Category=<the second category with hashtag>&subCategory=<the last category with hashtag>
// http://localhost:8585/Search?type=builder&mainCategory=Fintech&Category=StockExchange&subCategory=Forex

app.post('/api/search', function (req, res) {
    MongoClient.connect(urlPOST, function(err, db) {
        if (err) throw err;
        var dbo = db.db("Searchs");
        //var myobj = { type:req.body.type, mainCategory:req.body.mainCategory,Category:req.body.Category,subCategory:req.body.subCategory};
        var myobj = { type:"builder", mainCategory:"Fintech",Category:"StockExchange",subCategory:"Forex"};
        dbo.collection("Search").insertOne(myobj, function(err, result) {
          if (err) throw err;
          console.log(result);
          db.close();
          res.send(JSON.stringify(result))
        });
      }); 
  })

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
})