import React from 'react';
import ReactLoading from 'react-loading';

export const LoadingSymbol = () => {
	return (
		<div className={'d-flex justify-content-center align-items-center'}>
			<ReactLoading type={'spin'} color="#1890ff" />
		</div>
	);
};
