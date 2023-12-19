
const express = require('express');
const DataBaseConnector = require('./src/Services/DataBaseConnector');

const app = express();

// Initialize database connection
DataBaseConnector.connect();

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});