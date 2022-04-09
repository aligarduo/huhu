/**
 * 数字每隔三位逗号分隔
 * @param {int} number 
 * @returns 分隔结果
 */
const number_divide = (number) => {
    //记录是正值还是负值
    let heatos = "";
    if (number < 0) {
        heatos = "-";
        //将负数转正,如果不转正，则下面获取它的length时，会由于有个负号，使得长度+1，最终加逗号位置出错
        number = -number;
    }
    //将数字转化为了数组，便于使用数组中的splice方法插入逗号
    let result = number.toString().split("");
    //获取小数点的位置，根据有无小数点确定position最终值进入添加逗号环节
    let position = result.indexOf(".");
    //因为只需考虑整数部分插入逗号，所以需要考虑有没有逗号。有逗号则不等于-1，减去逗号位置，则是下标0~position就是整数部分；若不是小数，则number原本就是整数，直接取其length即可
    position = position !== -1 ? position -= 1 : result.length;
    //只要下标值大于3，说明前面还可以插入','，则继续循环
    while (position > 3) {
        position -= 3; //下标前移4位，然后在这个下标对应的元素后面插入逗号
        result.splice(position + 1, 0, ",");
    }
    //数组转换为字符串,前面+heatos，若为负数则拼接个符号，否则拼接空字符
    return heatos + result.join("");
}

/**
 * 数字超过一万保留一位小数变成W
 * @param {int} number 
 * @returns 结果
 */
const number_thousands = (number) => {
    number = parseInt(number);
    return number > 10000 ? (((number - number % 1000) / 10000 + 'W')) : (number);
}

/**
 * 超过一万则保留一位小数变成W，否则数字每隔三位逗号分隔
 * @param {*} number 
 * @returns 数字每隔三位逗号分隔or数字超过一万保留一位小数变成W
 */
const number_divide_thousands = (number) => {
    number = parseInt(number);
    return number > 10000 ? number_divide((number - number % 1000) / 10000) + 'W' : number_divide(number);
}


const numbers_help = () => {
    return {
        number_divide,
        number_thousands,
        number_divide_thousands
    }
}
export default numbers_help;