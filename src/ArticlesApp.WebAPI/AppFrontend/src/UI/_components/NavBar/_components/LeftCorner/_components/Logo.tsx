import { styled } from '@material-ui/core';



export function Logo() {
  return (
    <Div>
      <Header>
        Articles app
      </Header>
    </Div >
  )
}



const Div = styled("div")({
  height: "100%",
  display: "flex",
  alignItems: "center"
})

const Header = styled("h1")({
  margin: "-2px 0 0 2px",
  color: "forestgreen",
  fontSize: "1.7em",
  fontFamily: "'Open Sans', sans-serif",
  userSelect: "none"
})