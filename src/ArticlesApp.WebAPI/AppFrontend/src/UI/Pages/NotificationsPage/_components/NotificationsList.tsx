import { useSelector } from "react-redux";
import { storeType } from "src/Store/store";
import { List } from "UI/Pages/_components/List";
import { CardComponent } from "./CardComponent";



export function NotificationsList() {
  let notifications = useSelector((state: storeType) => state.notifications.notifications.entities);
  if (notifications === undefined) {
    notifications = [];
  }

  return (
    <List
      title={"My notifications"}
      entities={notifications}
      render={(ownProps) => (
        <CardComponent
          entity={ownProps.entity}
        />
      )}
    />
  )
}