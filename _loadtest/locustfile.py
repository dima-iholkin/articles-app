import time
from locust import HttpUser, task, between

import warnings
warnings.filterwarnings("ignore")



class QuickstartUser(HttpUser):
    wait_time = between(1, 2.5)

    @task
    def get_articles(self):
        self.client.get("/api/articles")

    # @task(3)
    # def view_items(self):
    #     for item_id in range(10):
    #         self.client.get(f"/item?id={item_id}", name="/item")
    #         time.sleep(1)

    def on_start(self):
      self.client.verify = False
    #     self.client.post("/login", json={"username":"foo", "password":"bar"})