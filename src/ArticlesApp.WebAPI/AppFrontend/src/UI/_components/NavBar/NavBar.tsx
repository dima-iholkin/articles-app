import { LeftCorner } from "./_components/LeftCorner/LeftCorner";
import { RightCorner } from "./_components/RightCorner/RightCorner";
import { TopBar } from "./_components/TopBar";



export function NavBar() {
  return (
    <TopBar>
      <LeftCorner />
      <div />
      <RightCorner />
    </TopBar>
  )
}