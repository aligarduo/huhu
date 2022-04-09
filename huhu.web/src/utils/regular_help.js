
const regularPhone = (phone) => {
    if(!(/^1(3|4|5|6|7|8|9)\d{9}$/.test(phone))){ 
        return false; 
    } 
    return true;
}

const regular_help = () => {
    return {
        regularPhone,
    }
}
export default regular_help;
