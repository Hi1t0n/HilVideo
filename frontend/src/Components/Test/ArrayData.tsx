const products = [
    { title: 'Cabbage', isFruct: false,id: 1 },
    { title: 'Garlic',isFruct: false, id: 2 },
    { title: 'Apple',isFruct: true, id: 3 },
];

const listItems = products.map(product=>
    <li key={product.id} style={{color: product.isFruct ? "magenta" : "red"}}>
        {product.title}
        </li>
);

function ArrayData(){
    return(
        <ul>
            {listItems}
        </ul>
    );
}

export default ArrayData;