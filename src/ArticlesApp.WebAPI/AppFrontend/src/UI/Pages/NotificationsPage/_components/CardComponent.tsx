import { Card, CardContent, Typography } from "@material-ui/core";
import Chip from '@material-ui/core/Chip';
import { makeStyles, styled } from '@material-ui/styles';
import { Notification } from "src/Entities/Notification";
import { markNotificationAsRead } from "src/Store/reducers/notificationsSlice";
import { store } from "src/Store/store";
import { RenderProps } from "UI/Pages/_components/List";



export function CardComponent(props: RenderProps<Notification>) {
  const classes = useStyles();
  const typographyClasses = typographyTitle();
  const chipClasses = useChipClasses();

  const handleClick = (notifId: number) => {
    store.dispatch(markNotificationAsRead(notifId) as any);
  }



  return (
    <Card
      classes={classes}
      onClick={() => void (undefined)} >
      {/* <LeftChipDiv /> */}
      {/* <MiddleDiv> */}
      <CardContent style={{
        paddingRight: "10px",
        paddingLeft: "10px",
        paddingBottom: "16px"
      }}>
        <Typography classes={typographyClasses}>
          {props.entity.message}
        </Typography>
      </CardContent>
      {/* </MiddleDiv> */}
      <RightChipDiv>
        {
          props.entity.readAt_DateUtc ? (
            <div />
          ) : (
            <Chip
              label="new"
              classes={chipClasses}
              style={{
                backgroundColor: "blue",
                marginRight: "5px",
                marginLeft: "10px",
              }}
              onClick={() => handleClick(props.entity.id)}
            />
          )
        }
      </RightChipDiv>
    </Card>
  )
}



const useStyles = makeStyles({
  root: {
    backgroundColor: "white",
    borderRadius: "0",
    // display: "grid",
    // gridTemplateColumns: "1fr auto 1fr",
    display: "flex",
    flexDirection: "row",
    justifyContent: "space-between",
    paddingBottom: "8px"
  }
});

const typographyTitle = makeStyles({
  body1: {
    fontFamily: "'Open Sans', sans-serif",
    textAlign: "left"
    // userSelect: "none",
    // whiteSpace: "nowrap",
    // overflow: "hidden",
    // textOverflow: "ellipsis"
  }
})

// const LeftChipDiv = styled("div")({
// });

// const MiddleDiv = styled("div")({
//   overflow: "hidden"
// });

const RightChipDiv = styled("div")({
  display: "flex",
  flexDirection: "row",
  justifyContent: "flex-end",
  alignItems: "center"
});

const useChipClasses = makeStyles({
  root: {
    color: "white"
  }
})