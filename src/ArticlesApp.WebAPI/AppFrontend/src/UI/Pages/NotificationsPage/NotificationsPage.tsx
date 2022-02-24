import { useLoginPrompt } from "UI/_components/LoginPromptProvider";
import { NotificationsList } from "./_components/NotificationsList";



export function NotificationsPage() {
  useLoginPrompt();

  return (
    <NotificationsList />
  )
}