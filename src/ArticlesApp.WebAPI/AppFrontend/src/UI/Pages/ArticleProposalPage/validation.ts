export function validateTheField(fieldValue?: string): boolean {
  if (fieldValue !== undefined && fieldValue !== "") {
    return true;
  } else {
    return false;
  }
}