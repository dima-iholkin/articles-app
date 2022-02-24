import React, { FunctionComponent } from 'react';
import { Box } from "@material-ui/core";
import { makeStyles, styled } from '@material-ui/styles';
import { IEntity } from "src/Entities/IEntity";
import { PageTitle } from './PageTitle';



interface Props<T> {
  title: string,
  entities: T[],
  render: FunctionComponent<RenderProps<T>>
}

export interface RenderProps<TR> {
  entity: TR
}

export function List<T extends IEntity>(props: Props<T>) {
  const classes2 = useStyles2();

  return (
    <ListComponent>
      <PageTitle>{props.title}</PageTitle>
      {props.entities.map((ar: T) => {
        return (
          <Box key={ar.id} boxShadow={1} className={classes2.box}>
            {React.createElement<RenderProps<T>>(props.render, { entity: ar })}
          </Box>
        )
      })}
    </ListComponent>
  )
}



const ListComponent = styled("div")({
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  justifyContent: "start",
  marginTop: "0",
  paddingBottom: "32px"
})

const useStyles2 = makeStyles({
  box: {
    margin: "0 0 8px 0"
  }
});