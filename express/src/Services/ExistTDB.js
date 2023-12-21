const mysql = require('mysql');
const ConfigReaderHelper = require('../Helper/ConfigReaderHelper');

class ExistTDB {
  static E_EventoTable() {
    try {
      const config = ConfigReaderHelper.readConfig().database;
      const connection = mysql.createConnection(config);

      connection.connect((err) => {
        if (err) {
          console.log('\n--------------------------------------------');
          console.error('\nError al Conectar a la base de datos (if):', err.message);
          console.log('\n--------------------------------------------');
          throw err;
        }

        console.log('\n-----------------------------------------');
        console.log('\nConexion a la Base de Datos Exitosa.');
        console.log('\n-----------------------------------------');

        // Verificar la existencia de la tabla act_Eventos
        const query = "SHOW TABLES LIKE 'act_Eventos'";
        connection.query(query, (err, results) => {
          if (err) {
            console.error('\nError al consultar la existencia de la tabla:', err.message);
            connection.end();
            return false;
          }

          if (results.length > 0) {
            console.log('\n-----------------------------------------');
            console.log('\nLa tabla "act_Eventos" Existe.');
            console.log('\n-----------------------------------------');
            connection.end();
            return true;
          } else {
            console.log('\n-----------------------------------------');
            console.log('\nLa tabla "act_Eventos" No Existe.');
            console.log('\n-----------------------------------------');
            connection.end();
            return false;
          }
        });
      });
    } catch (error) {
      console.log('\n---------------------------------------------------');
      console.error('Erro al conectar a la Base de Datos (try-catch):', error.message);
      console.log('\n---------------------------------------------------');
      return false;
    }
  }
}

module.exports = ExistTDB;
