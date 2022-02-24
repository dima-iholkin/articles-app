SQL Server docker image:
> docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>" -p 1433:1433 --name sqlserver1 -h sqlserver1  -
v sqlserver1:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-CU14-ubuntu-20.04
* Name: sqlserver1
* Volume: sqlserver1
* IP address: 192.168.99.105
* Port: 1433
* Login: SA
* Password: <YourStrong!Passw0rd>
* Database name: ArticlesApp_Staging
* Connection string: Data Source=192.168.99.105,1433;Database=ArticlesApp_Staging;User ID=SA;Password=<YourStrong!Passw0rd>;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

Get docker-machine IP address:
> docker-machine ip

ElasticSearch docker image:
> docker run -d --name elasticsearch1 --net elasticsearch1 -e "ES_JAVA_OPTS=-Xms256m -Xmx256m" -v elasticsearch1:/usr/share/elasticsearch/data -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.16.2
Wait for readiness:
> curl -X GET "192.168.99.105:9200/_cluster/health?pretty=true&wait_for_status=green"

Kibana docker image:
> docker run -d --name kibana1 --net elasticsearch1 -e "ELASTICSEARCH_HOSTS=http://192.168.99.105:9200" -v kibana1_config:/usr/share/kibana/config -v kibana1_data:/usr/share/kibana/data -p 5601:5601 kibana:7.16.2

Prometheus docker image:
> docker run --name prometheus1 --network host -v /d/data/prometheus.yml:/etc/prometheus/prometheus.yml -v prometheus1:/prometheus -d prom/prometheus:v2.32.1
* host or gateway IP: 10.0.2.2
* ports are not specified in the host network mode:  -p 9090:9090

Redis docker image:
> docker run --name redis1 -h redis1 -v redis1:/data -p 6379:6379 -d redis:6.2.6 redis-server --save 60 1 --loglevel warning