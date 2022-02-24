import Chip, { ChipProps } from '@material-ui/core/Chip';
import { styled } from '@material-ui/styles';
import { ArticleStatesEnum } from "src/Entities/ArticleStatesEnum";



interface Props {
  articleState: ArticleStatesEnum
}

export function ArticleStateChip(props: Props) {
  const articleState = props.articleState;

  let chipColor: string;
  switch (articleState) {
    case ArticleStatesEnum.Approved:
      chipColor = "green";
      break;
    case ArticleStatesEnum.Pending:
      chipColor = "#CCCC00";
      break;
    case ArticleStatesEnum.Rejected:
      chipColor = "darkred";
      break;
    default:
      break;
  }

  return (
    <ChipStyled
      label={ArticleStatesEnum[articleState]}
      backColor={chipColor!}
    />
  )
}



interface ChipStyledProps extends ChipProps {
  backColor: string
}

// remove the custom property, to avoid warning on passing it as the DOM node attribute.
const ChipStyled = styled(({ backColor, ...propsWithoutBackColor }: ChipStyledProps) =>
  <Chip {...propsWithoutBackColor} />
)(
  (props: ChipStyledProps) => ({
    color: "white",
    marginRight: "5px",
    marginLeft: "10px",
    backgroundColor: props.backColor
  })
)