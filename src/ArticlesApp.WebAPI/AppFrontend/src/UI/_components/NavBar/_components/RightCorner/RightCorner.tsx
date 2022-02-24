import { styled } from "@material-ui/styles";
import { MenuButton } from "./_components/MenuButton";



export function RightCorner() {
  return (
    <Div>
      <MenuButton />
    </Div>
  )
}



const Div = styled("div")({
  width: "auto",
  display: "flex",
  flexDirection: "row",
  justifyContent: "flex-end"
});