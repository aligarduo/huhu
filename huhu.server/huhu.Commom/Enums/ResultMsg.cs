using huhu.Commom.Enums.model;

namespace huhu.Commom.Enums
{
    public class ResultMsg
    {
        public state SetResultMsg(int err_no, string err_msg)
        {
            state msg = new state {
                err_no = err_no,
                err_msg = err_msg
            };
            return msg;
        }

        public tokens SetResultToken(int err_no, string err_msg, string token)
        {
            tokens msg = new tokens {
                err_no = err_no,
                err_msg = err_msg,
                token = token
            };
            return msg;
        }

        public detail SetResultMsg(int err_no, string err_msg, object data)
        {
            detail msg = new detail {
                err_no = err_no,
                err_msg = err_msg,
                data = data
            };
            return msg;
        }

        public datas SetResultMsg(int err_no, string err_msg, int count, string cursor, object data, bool has_more)
        {
            datas msg = new datas {
                err_no = err_no,
                err_msg = err_msg,
                count = count,
                cursor = cursor,
                data = data,
                has_more = has_more
            };
            return msg;
        }

    }
}