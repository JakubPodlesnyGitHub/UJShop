import useGetRequest from "./GetRequest"

export default function ProductDescription(props) {
  const data = useGetRequest(`http://localhost:8080/product/${props["id"]}`)

  return (
    <>
      <h1>{data["name"]}</h1>
      <h2>{data["description"]}</h2>
    </>
  );
}