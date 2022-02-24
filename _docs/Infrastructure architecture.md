Client → URL+DNS+TLS → LB ⇶ AppBackends

<br/><br/>

### Infrastructure services:

|            Service             |            Local             |                GCP                 |
| :----------------------------: | :--------------------------: | :--------------------------------: |
|            Database            |          SQL Server          |           GCP Cloud SQL            |
|              Logs              |    ElasticSearch + Kibana    | GCP Stackdriver (Cloud Operations) |
|            Metrics             |     Prometheus + Grafana     | GCP Stackdriver (Cloud Operations) |
|           Analytics            |    ELasticSearch + Kibana    |                                    |
| Third-party Identity providers |       GitHub accounts        |          GitHub accounts           |
|            Host OS             |    Windows/Linux machine     |           GCP App Engine           |
|        Top-level domain        | Freenom Tokelau free domains |    Freenom Tokelau free domains    |
|               LB               |                              |   GCP App Engine / Load Balancer   |
|          DNS hosting           |                              |           GCP Cloud DNS            |
|       Secrets and config       |  host OS env vars and files  |         GCP Secret Manager         |